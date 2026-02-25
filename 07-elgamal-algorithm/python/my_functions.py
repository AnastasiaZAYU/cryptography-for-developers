# Implementation of the ElGamal algorithm for digital signature and encryption

from Crypto.Util.number import getPrime, inverse
from random import randint
from math import gcd
import hashlib

def generate_parameters(size = 2048):
  """Generates a large prime number p and a generator g for the field."""
  p = getPrime(size)
  while True:
    g = randint(2, p - 1)
    if pow(g, (p - 1) // 2, p) != 1:
      return p, g

def generate_key_pair(p, g):
  """Generates a private and public key pair."""
  private_key = randint(2, p - 2)
  public_key = pow(g, private_key, p)
  return private_key, public_key

def sign(message, p, g, private_key):
  """Signs a message using the private key 'a'."""
  Hm = int(hashlib.sha256(message.encode()).hexdigest(), 16)
  while True:
    k = randint(2, p - 2)
    if gcd(k, p - 1) == 1:
      try:
        k_inv = inverse(k, p - 1)
        r = pow(g, k, p)
        s = (k_inv * (Hm - private_key * r)) % (p - 1)
        if s != 0:
          return r, s
      except ValueError:
        continue

def verify(message, r, s, b, p, g):
  """Verifies the digital signature (r, s) using the public key 'b'."""
  if not (0 < r < p and 0 < s < p - 1):
    return False
  Hm = int(hashlib.sha256(message.encode()).hexdigest(), 16)
  v1 = pow(g, Hm, p)
  v2 = (pow(b, r, p) * pow(r, s, p)) % p
  return v1 == v2

def encrypt(message, b, p, g):
  """Encrypts a message using the recipient's public key 'b'."""
  k = randint(2, p - 2)
  x = pow(g, k, p)
  shared_secret = pow(b, k, p)
  byte_size = (p.bit_length() - 1) // 8
  msg_bytes = message.encode('utf-8')
  blocks = [msg_bytes[i:i + byte_size] for i in range(0, len(msg_bytes), byte_size)]
  y_elements = []
  for block in blocks:
    m_int = int.from_bytes(block, byteorder='big')
    y_elements.append(hex((m_int * shared_secret) % p)[2:])
  y_joined = ":".join(y_elements)
  return x, y_joined

def decrypt(x, y_joined, private_key, p):
  """Decrypts the ciphertext (x, y) using the private key."""
  shared_secret = pow(x, private_key, p)
  s_inv = inverse(shared_secret, p)
  y_elements = y_joined.split(":")
  decrypted_bytes = bytearray()
  for y_hex in y_elements:
    y_int = int(y_hex, 16)
    m_int = (y_int * s_inv) % p
    needed_bytes = (m_int.bit_length() + 7) // 8
    decrypted_bytes.extend(m_int.to_bytes(needed_bytes, byteorder='big'))
  return decrypted_bytes.decode('utf-8', errors='ignore')