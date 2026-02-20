# Практичне завдання: «Реалізація алгоритму цифрового підпису»

**Завдання:**
1. Реалізація RSA, або ECDSA, або Schnorr, або Ring traceable signatures.
2. Тестування та демонстрація роботи.

**Деталі реалізації**
Для реалізації цифрового підпису було обрано алгоритм ECDSA.

Клас ECDSA складається з 6 функцій:
1. (AsymmetricKeyParameter, AsymmetricKeyParameter) KeyGen() -- генерує пару ключів;
2. byte[] Hash(string message) -- приймає повідомлення і повертає геш-значення;
3. byte[] Sign(string message, AsymmetricKeyParameter sk) -- приймає повідомлення та приватний ключ, і повертає підпис;
4. bool Verify(string message, byte[] signature, AsymmetricKeyParameter pk) -- приймає повідомлення, підпис та публічний ключ, і повертає  False, якщо було пошкоджено підпис (або повідомлення, або ключ), інакше -- True;
5. string ConvertToPEM(AsymmetricKeyParameter key) -- повертає ключ у форматі PEM;
6. AsymmetricKeyParameter InputKey() -- повертає ключ із формату PEM.

**Інструкція щодо запуску:**
1. Запустити проєкт Digital_Signature.
2. Виконати одну з наступних дій:
    1. Натиснiть "0" + Enter - щоб згенерувати пару ключів;

![0](https://github.com/AnastasiaZAYU/Cryptography-Course/blob/main/Digital_Signature/screenshots/0.png)

    2. Натиснiть "1" + Enter. Потім вводимо повідомлення і отримуємо геш повiдомлення;

![1](https://github.com/AnastasiaZAYU/Cryptography-Course/blob/main/Digital_Signature/screenshots/1.png)

    3. Натиснiть "2" + Enter. Потім вводимо повідомлення, приватний ключ і отримуємо підпис до повідомлення;

![2](https://github.com/AnastasiaZAYU/Cryptography-Course/blob/main/Digital_Signature/screenshots/2.png)

    4. Натиснiть "3" + Enter. Потім вводимо повідомлення, відкритий ключ, підпис і отримуємо відповідь, щодо цілісність та авторство даних;

![3](https://github.com/AnastasiaZAYU/Cryptography-Course/blob/main/Digital_Signature/screenshots/3.png)

    5. Натиснiсть будь-який iнший символ - щоб завершити роботу.

Перевіримо роботу функції Verify на пошкодженому повідомленні:

![4](https://github.com/AnastasiaZAYU/Cryptography-Course/blob/main/Digital_Signature/screenshots/4.png)

 Отже, дана реалізація алгоритму ECDSA працює коректно.
