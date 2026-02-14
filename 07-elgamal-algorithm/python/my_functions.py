# Реалізація алгоритму Ель-Гамаля для підписання та шифрування повідомлень

# Підключення бібліотек
from Crypto.Util.number import getPrime
from random import randint
import hashlib

# Генерація простого числа та генератора поля
p = 0
g = 0

def generate_parameters(size):
  global p, g
  p = getPrime(size)
  while True:
        g = randint(2, p - 1)
        if pow(g, 2, p) != 1 and pow(g, p - 1, p) == 1:
            return p, g

 # Генерація ключової пари
def generate_key_pair():
  global p, g
  private_key = randint(1, p - 1)
  public_key = pow(g, private_key, p)
  return private_key, public_key

# Підписання повідомлення
def sign(message, a):
  global p, g
  Hm = int(hashlib.sha256(message.encode()).hexdigest(), 16)
  while True:
    k = randint(1, p - 2)
    if can_modinv(k ,p - 1):
      r = pow(g, k, p)
      s = (pow(k, -1, p - 1) * (Hm - a * r)) % (p - 1)
      if can_modinv(s, p - 1):
        return r, s

# Перевірка підпису
def verify(message, r, s, b):
  global p, g
  Hm = int(hashlib.sha256(message.encode()).hexdigest(), 16)
  y = pow(b, -1, p)
  s1 = pow(s, -1, p - 1)
  u1 = (Hm * s1) % (p - 1)
  u2 = (r * s1) % (p - 1)
  v = (pow(g, u1, p) * pow(y, u2, p)) % p
  if v == r:
    return True
  return False

# Шифрування
def encrypt(message, b):
  global p, g
  k = randint(1, p - 1)
  x = pow(g, k, p)
  y1 = pow(b, k, p)
  block_size = (p.bit_length() + 7) // 8
  blocks = split_message(message, block_size, 0)
  y = ""
  for part in blocks:
    m = int.from_bytes(part, byteorder='big')
    y2 = (y1 * m) % p
    y += hex(y2)[2:].zfill(block_size * 2)
  return x, y

# Розшифрування
def decrypt(x, y, private_key):
  global p, g
  s = pow(x, private_key, p)
  m1 = pow(s, -1, p)
  block_size = (p.bit_length() + 7) // 8
  blocks = split_message(y, block_size, 1)
  text = ""
  for part in blocks:
    item = int.from_bytes(part, byteorder='big')
    m = (item * m1) % p
    converted_bytes = m.to_bytes((m.bit_length() + 7) // 8, byteorder='big')
    text += converted_bytes.decode('utf-8')
  return text

# Функція перевірки існування оберненого
def can_modinv(k, p):
    try:
        pow(k, -1, p)
        return True
    except ValueError:
        return False

# Функція розбиття повідомлення на блоки
def split_message(message, block_size, state):
  if state == 0:
    byte_representation = message.encode('utf-8')
  if state == 1:
    byte_representation = bytes.fromhex(message)
  blocks = [byte_representation[i:i + block_size] for i in range(0, len(byte_representation), block_size)]
  return blocks
