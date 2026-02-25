# ElGamal Cryptosystem (Python Implementation)

This directory contains a complete Python implementation of the ElGamal cryptosystem, featuring both **Digital Signature** and **Asymmetric Encryption** with automatic message blocking.

## ⚡ Key Features

### 🧪 Automated Reliability Tests
The project includes a suite of automated tests to ensure mathematical correctness:
- **Signature Integrity**: Verifies that valid signatures pass and modified messages fail.
- **Block Encryption**: Validates the logic for messages longer than the prime modulus $p$ (multi-block processing).
- **Key Consistency**: Ensures generated public/private keys are mathematically linked.

### 🖥️ Interactive Demonstration
The CLI supports the following commands:
- `gen_parameters`: Generate prime field modulus $p$ and generator $g$.
- `gen_keys`: Create a new public/private key pair.
- `sign` / `verify`: Create and validate digital signatures using SHA-256.
- `encrypt` / `decrypt`: Perform asymmetric encryption on messages of any length.

## 📁 Project Structure
```
├── elgamal_demo.ipynb   # Google Colab demo
├── my_console.py        # CLI for manual interaction
├── my_functions.py      # Сore algorithm logic
├── README.md
└── requirements.txt     # Project dependencies
```

## 🚀 How to Run 

### 🌐 Run via Google Colab
The easiest way to test the implementation is through Google Colab:

[![Open In Colab](https://colab.research.google.com/assets/colab-badge.svg)](https://colab.research.google.com/github/AnastasiaZAYU/cryptography-for-developers/blob/main/07-elgamal-algorithm/python/elgamal_demo.ipynb)

> **Note**: The notebook is pre-configured to clone the repository and install all dependencies automatically.

### 💻 Local Installation
Ensure you have Python 3.8+ installed.

1. **Install dependencies:**
```bash
pip install -r requirements.txt
```
2. **Launch the interactive console:**
```bash
python my_console.py
```

## 🛠️ Tech Stack
- **Python 3**
- **PyCryptodome**: Used for secure large prime generation and modular inverse operations.
- **Hashlib**: Provides SHA-256 for message hashing.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.
