# ElGamal Cryptosystem Implementation

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This directory contains a cross-platform implementation of the **ElGamal Cryptosystem** in Python and C#. The project demonstrates the two primary applications of the algorithm: **Digital Signatures** and **Asymmetric Encryption**.

## 📌 Overview

The ElGamal algorithm is an asymmetric key encryption scheme based on the **Diffie-Hellman key exchange**. Its security relies on the difficulty of computing **discrete logarithms** in a large finite field.

### Key Components:
1. **Parameter Generation**: Selection of a large prime $p$ and a generator $g$ of the multiplicative group $\mathbb{Z}_p^*$.
2. **Key Generation**: Creating a private key $x$ and a corresponding public key $y = g^x \pmod p$.
3. **Digital Signature**: Creating a pair $(r, s)$ that proves the authenticity of a message.
4. **Encryption**: Transforming a message into a pair $(c_1, c_2)$ using the recipient's public key.

## 📁 Repository Structure

The project is divided into two independent implementations, each following the best practices of its respective ecosystem:

### 🐍 [Python Implementation](./python)
- **Focus**: Rapid prototyping and interactive testing.
- **Features**: Jupyter Notebook (Google Colab) support, `pycryptodome` integration, and a lightweight CLI.

### ⚡ [C# Implementation](./csharp)
- **Focus**: Robustness, performance, and industrial-grade architecture.
- **Features**: Custom Miller-Rabin primality test, .NET 10, xUnit testing suite, and strong type safety.

## ⚙️ Feature Comparison

| Feature | 🐍 Python | ⚡ C# |
| :--- | :--- | :--- |
| **Prime Generation** | PyCryptodome (`getPrime`) | Custom Miller-Rabin |
| **Big Number Support** | Native Python `int` | `System.Numerics.BigInteger` |
| **Message Blocking** | Manual byte-slicing | Big-endian byte management |
| **Hashing** | SHA-256 (`hashlib`) | SHA-256 (`System.Security`) |
| **Interface** | Google Colab / CLI | Console App / API |
| **Unit Testing** | Inline automated tests | xUnit Framework |

## 🛠 Mathematical Foundation
The implementation ensures the following mathematical properties:
- **Signature Verification**: $g^M \equiv y^r \cdot r^s \pmod p$
- **Decryption**: $M = c_2 \cdot (c_1^x)^{-1} \pmod p$

## 🚀 Quick Start
To explore a specific implementation, navigate to its directory and follow the local `README.md` instructions:
1. **For Python**: Go to [/python](./python) to run the Colab notebook.
2. **For C#**: Go to [/csharp](./csharp) to open the `.slnx` solution in Visual Studio.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.
