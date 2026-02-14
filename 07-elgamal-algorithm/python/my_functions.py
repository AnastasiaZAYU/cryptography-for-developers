# Implementation of the ElGamal algorithm for digital signature and encryption

from Crypto.Util.number import getPrime
from random import randint
import hashlib

# Global variables for system parameters
p = 0
g = 0

def generate_parameters(size):
  """Generates a large prime number p and a generator g for the field."""
  global p, g
  p = getPrime(size)
  while True:
    g = randint(2, p - 1)
    if pow(g, 2, p) != 1 and pow(g, p - 1, p) == 1:
      return p, g

def generate_key_pair():
  """Generates a private and public key pair."""
  global p, g
  private_key = randint(1, p - 1)
  public_key = pow(g, private_key, p)
  return private_key, public_key

def sign(message, a):
  """Signs a message using the private key 'a'."""
  global p, g
  Hm = int(hashlib.sha256(message.encode()).hexdigest(), 16)
  while True:
    k = randint(1, p - 2)
    if can_modinv(k, p - 1):
      r = pow(g, k, p)
      s = (pow(k, -1, p - 1) * (Hm - a * r)) % (p - 1)
      if s != 0 and can_modinv(s, p - 1):
        return r, s

def verify(message, r, s, b):
  """Verifies the digital signature (r, s) using the public key 'b'."""
  global p, g
  if not (0 < r < p and 0 < s < p - 1):
    return False

  Hm = int(hashlib.sha256(message.encode()).hexdigest(), 16)
  y = pow(b, -1, p)
  s1 = pow(s, -1, p - 1)
  u1 = (Hm * s1) % (p - 1)
  u2 = (r * s1) % (p - 1)
  v = (pow(g, u1, p) * pow(y, u2, p)) % p
  return v == r

def encrypt(message, b):
  """Encrypts a message using the recipient's public key 'b'."""
  global p, g
  k = randint(1, p - 2)
  x = pow(g, k, p)
  y1 = pow(b, k, p)
  block_size = (p.bit_length() - 1) // 8
  blocks = split_message(message, block_size, 0)
  y = ""
  hex_block_size = (p.bit_length() + 7) // 8 * 2
  for part in blocks:
    m = int.from_bytes(part, byteorder='big')
    y2 = (y1 * m) % p
    y += hex(y2)[2:].zfill(hex_block_size)
  return x, y

def decrypt(x, y, private_key):
  """Decrypts the ciphertext (x, y) using the private key."""
  global p, g
  s = pow(x, private_key, p)
  m1 = pow(s, -1, p)
  hex_block_size = (p.bit_length() + 7) // 8 * 2
  blocks = split_message(y, hex_block_size, 1)
  text = ""
  for part in blocks:
    item = int.from_bytes(part, byteorder='big')
    m = (item * m1) % p
    converted_bytes = m.to_bytes((m.bit_length() + 7) // 8, byteorder='big')
    text += converted_bytes.decode('utf-8')
  return text

def can_modinv(k, p):
  """Checks if the modular inverse of k modulo n exists."""
  try:
    pow(k, -1, p)
    return True
  except ValueError:
    return False

def split_message(message, block_size, state):
  """Splits a message into blocks of a specified size."""
  if state == 0:
    byte_representation = message.encode('utf-8')
  else:
    byte_representation = bytes.fromhex(message)
  return [byte_representation[i:i + block_size] for i in range(0, len(byte_representation), block_size)]
