import cmd
from my_functions import *

# Class to handle the ElGamal algorithm CLI
class MyConsole(cmd.Cmd):
  intro = "Enter 'help' або '?' to list commands.\nBefore starting, generate algorithm parameters using 'gen_parameters'.\nTo exit, enter 'quit'."
  prompt = '> '

  def do_gen_parameters(self, arg):
    """Generates the prime field modulus (p) and generator (g)."""
    try:
      size = int(input("Enter field modulus size in bits (e.g., 2048): "))
      p, g = generate_parameters(size)
      print("Field modulus (p): ", hex(p))
      print("Field generator (g): ", hex(g))
    except ValueError:
      print("Error: Please enter a valid number.")

  def do_gen_keys(self, arg):
    """Generates private and public key pair."""
    try:
      a, b = generate_key_pair()
      print("Private key (a): ", hex(a))
      print("Public key (b): ", hex(b))
    except ValueError:
      print("Error: Something went wrong. Ensure parameters are generated.")

  def do_sign(self, arg):
    """Signs a message using the private key."""
    try:
      message = input("Enter message to sign: ")
      a = int(input("Enter your private key (hex): "), 16)
      r, s = sign(message, a)
      print("Message signature:")
      print("r: ", hex(r))
      print("s: ", hex(s))
    except ValueError:
      print("Error: Invalid key format.")

  def do_verify(self, arg):
    """Verifies a digital signature."""
    try:
      message = input("Enter the original message: ")
      print("Enter signature components:")
      r = int(input("r (hex): "), 16)
      s = int(input("s (hex): "), 16)
      b = int(input("Enter public key (hex): "), 16)
      is_valid = verify(message, r, s, b)
      print(is_valid)
    except ValueError:
      print("Error: Invalid input format.")

  def do_encrypt(self, arg):
    """Encrypts a message using the public key."""
    try:
      message = input("Enter message to encrypt: ")
      b = int(input("Enter recipient's public key (hex): "), 16)
      x, y = encrypt(message, b)
      print("Ciphertext:")
      print("x (ephemeral component): ", hex(x))
      print("y (message blocks): ", y)
    except ValueError:
      print("Error: Invalid input.")

  def do_decrypt(self, arg):
    """Decrypts a ciphertext using the private key."""
    try:
      print("Enter ciphertext components: ")
      x = int(input("x (hex): "), 16)
      y = input("y (hex): ")
      a = int(input("Enter your private key (hex): "), 16)
      message = decrypt(x, y, a)
      print("Decrypted message: ", message)
    except Exception as e:
      print(f"Error during decryption: {e}")

  def do_quit(self, arg):
    """Exits the console."""
    return True
