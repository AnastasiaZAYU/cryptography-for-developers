import cmd
from my_functions import *

# Class to handle the ElGamal algorithm CLI
class MyConsole(cmd.Cmd):
  intro = "Enter 'help' або '?' to list commands.\nBefore starting, generate algorithm parameters using 'gen_parameters'.\nTo exit, enter 'quit'."
  prompt = '> '

  def __init__(self):
    super().__init__()
    self.p = None
    self.g = None

  def do_gen_parameters(self, arg):
    """Generates the prime field modulus (p) and generator (g)."""
    try:
      size = int(input("Enter field modulus size in bits (e.g., 2048): "))
      self.p, self.g = generate_parameters(size)
      print("Field modulus (p): ", hex(self.p))
      print("Field generator (g): ", hex(self.g))
    except ValueError:
      print("❌Error: Please enter a valid number.")

  def do_gen_keys(self, arg):
    """Generates private and public key pair."""
    try:
      a, b = generate_key_pair(self.p, self.g)
      print("Private key (a): ", hex(a))
      print("Public key (b): ", hex(b))
    except ValueError:
      print("❌Error: Something went wrong. Ensure parameters are generated.")

  def do_sign(self, arg):
    """Signs a message using the private key."""
    try:
      if not self.p or not self.g:
        self.p = int(input("Enter p (hex): "), 16)
        self.g = int(input("Enter g (hex): "), 16)
      message = input("Enter message to sign: ")
      a = int(input("Enter your private key (hex): "), 16)
      r, s = sign(message, self.p, self.g, a)
      print("Message signature:")
      print("r: ", hex(r))
      print("s: ", hex(s))
    except Exception as e:
      print(f"❌Error: {e}.")

  def do_verify(self, arg):
    """Verifies a digital signature."""
    try:
      if not self.p or not self.g:
        self.p = int(input("Enter p (hex): "), 16)
        self.g = int(input("Enter g (hex): "), 16)
      message = input("Enter the original message: ")
      print("Enter signature components:")
      r = int(input("r (hex): "), 16)
      s = int(input("s (hex): "), 16)
      b = int(input("Enter public key (hex): "), 16)
      is_valid = verify(message, r, s, b, self.p, self.g)
      print(f"{'✅True' if is_valid else '❌False'}")
    except Exception as e:
      print(f"❌Error: {e}.")

  def do_encrypt(self, arg):
    """Encrypts a message using the public key."""
    try:
      if not self.p or not self.g:
        self.p = int(input("Enter p (hex): "), 16)
        self.g = int(input("Enter g (hex): "), 16)
      message = input("Enter message to encrypt: ")
      b = int(input("Enter recipient's public key (hex): "), 16)
      x, y = encrypt(message, b, self.p, self.g)
      print("Ciphertext:")
      print("x (ephemeral component): ", hex(x))
      print("y (message blocks): ", y)
    except Exception as e:
      print(f"❌Error: {e}.")

  def do_decrypt(self, arg):
    """Decrypts a ciphertext using the private key."""
    try:
      if not self.p:
        self.p = int(input("Enter p (hex): "), 16)
      print("Enter ciphertext components: ")
      x = int(input("x (hex): "), 16)
      y = input("y (hex): ")
      a = int(input("Enter your private key (hex): "), 16)
      message = decrypt(x, y, a, self.p)
      print("Decrypted message: ", message)
    except Exception as e:
      print(f"❌Error: {e}.")

  def do_quit(self, arg):
    """Exits the console."""
    return True
