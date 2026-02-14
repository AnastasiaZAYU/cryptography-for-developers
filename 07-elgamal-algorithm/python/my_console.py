# Підключення бібліотек
import cmd
from my_functions import *

# Клас для виклику функцій
class MyConsole(cmd.Cmd):
  intro = "Введіть help або ? для отримання списку команд.\nПеред початком роботи потрібно згенерувати параметри алгоритму gen_parameters.\nДля того, щоб завершити роботу, введіть quit"
  prompt = '> '

  def do_gen_parameters(self, arg):
    try:
      size = int(input("Введіть розмір модуля поля в бітах: "))
      p, g = generate_parameters(size)
      print("Модуль поля: ", hex(p))
      print("Генератор поля: ", hex(g))
    except ValueError:
      print("Потрібно ввести число.")

  def do_gen_keys(self, arg):
    try:
      a, b = generate_key_pair()
      print("Приватний ключ: ", hex(a))
      print("Відкритий ключ: ", hex(b))
    except ValueError:
      print("Щось пішло  не так.")

  def do_sign(self, arg):
    try:
      message = input("Введіть повідомлення, яке хочете підписати: ")
      a = int(input("Введіть ваш приватний ключ: "), 16)
      r, s = sign(message, a)
      print("Підпис повідомлення:")
      print("r: ", hex(r))
      print("s: ", hex(s))
    except ValueError:
      print("Щось пішло не так.")

  def do_verify(self, arg):
    try:
      message = input("Введіть повідомлення, яке було підписано: ")
      print("Введіть підпис:")
      r = int(input("Введіть r: "), 16)
      s = int(input("Введіть s: "), 16)
      b = int(input("Введіть відкритий ключ: "), 16)
      ans = verify(message, r, s, b)
      print(ans)
    except ValueError:
      print("Щось пішло не так.")

  def do_encrypt(self, arg):
    try:
      message = input("Введіть повідомлення, яке хочете зашифрувати: ")
      b = int(input("Введіть відкритий ключ: "), 16)
      x, y = encrypt(message, b)
      print("Шифртекст:")
      print("x: ", hex(x))
      print("y: " + y)
    except ValueError:
      print("Щось пішло не так.")

  def do_decrypt(self, arg):
    try:
      print("Введіть шифртекст, який потрібно розшифруувати: ")
      x = int(input("Введіть x: "), 16)
      y = input("Введіть y: ")
      a = int(input("Введіть приватний ключ: "), 16)
      message = decrypt(x, y, a)
      print("Розшифроване повідомлення: ", message)
    except ValueError:
      print("Щось пішло не так.")

  def do_quit(self, arg):
      return True
