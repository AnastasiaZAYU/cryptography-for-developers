# Cryptography for Developers

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Welcome to the comprehensive repository for the **"Cryptography for Developers**" course. This project documents a complete journey from implementing low-level BigInt arithmetic to building sophisticated digital signature schemes and asymmetric encryption protocols.

## 🗺️ Project Roadmap
The repository is structured as a step-by-step progression, where each module builds upon the mathematical foundations established in the previous ones.

### 1. Fundamental Primitives
  - [**01-large-numbers**](./01-large-numbers): Custom `BigInt` library. Implementation of $a^e \pmod m$, bitwise operations, and schoolbook arithmetic.
  - [**02-s-block-p-block**](./02-s-block-p-block): Substitution-Permutation networks. Demonstrating the core principles of **Confusion** and **Diffusion**.
  - [**03-randomness-testing**](./03-randomness-testing): **Blum Blum Shub (BBS)** generator and a full suite of **FIPS 140** statistical tests.

### 2. Hashing & Elliptic Curves
  - [**04-sha1-hash**](./04-sha1-hash): High-performance SHA-1 implementation. Optimized with `Span<T>` and `stackalloc` for a **Zero-GC** footprint.
  - [**05-elliptic-curves**](./05-elliptic-curves): ECC Wrapper. Point arithmetic on the `secp256k1` curve, including point addition, doubling, and scalar multiplication.

### 3. Cryptosystems & Digital Signatures
  - [**06-digital-signature**](./06-digital-signature): Full **ECDSA** implementation. Manual logic for key generation, message signing, and signature verification.
  - [**07-elgamal-algorithm**](./07-elgamal-algorithm): **ElGamal** asymmetric encryption and signatures.
    - **C# Version**: Production-grade architecture with block management and Miller-Rabin primality testing.
    - **Python Version**: Interactive prototyping using Jupyter Notebooks (Google Colab).
   
## 🛠 Tech Stack & Key Features

| Category | Technologies |
| :--- | :--- |
| **Languages** |	C# 12 (.NET 8/10), Python 3.12 |
| **Testing** |	xUnit, Automated Differential Testing |
| **Performance** |	BenchmarkDotNet, SIMD-friendly patterns, `ReadOnlySpan<byte>` |
| **Security** | CSPRNG (RandomNumberGenerator), Miller-Rabin Primality Test |
| **Integrations** |	BouncyCastle (ECC math core), PyCryptodome |

## 🚀 Getting Started
1. **Clone the repository**:

```bash
git clone https://github.com/AnastasiaZAYU/cryptography-for-developers.git
```

2. **For .NET Projects**:
 
Open the respective `.slnx` file in **[Visual Studio](https://visualstudio.microsoft.com/)** (2022 or newer) installed with the **.NET desktop development** workload.

3. **For Python Projects (ElGamal)**:
 
Run the Colab notebook or via CLI:
```bash
cd 07-elgamal-algorithm/python
pip install -r requirements.txt
python my_console.py
```

## 🧪 Validation & Reliability
Every algorithm in this repository has undergone rigorous verification:
- **Mathematical Accuracy**: Cross-validated against `System.Numerics` and `System.Security.Cryptography`.
- **Edge Case Handling**: Extensive testing for overflows, null vectors, and the _Point at Infinity_.
- **Standard Compliance**: Random number generators are validated against FIPS 140 statistical intervals.

## 🧠 Key Competencies Developed
- Implementation of complex mathematical algorithms (Modular Inverse, Miller-Rabin, ECC Point Arithmetic).
- High-performance .NET coding practices (Memory-safe pointers, Span-based buffer management).
- Test-Driven Development (TDD) for mission-critical cryptographic logic.

## 🎓 Certification

I have successfully completed the **"Cryptography for Developers"** course by **Distributed Lab**.

[![Cryptography Certificate](./assets/certificate_Anastasiia_Zatsarenko.jpg)](./assets/certificate_Anastasiia_Zatsarenko.pdf)

*Click on the image to view the full PDF certificate.*

* **Credential ID**: `9287ea51d845c9333dff`
* **Grade**: 223/245 points (91%)
* **Issued on**: December 27, 2023
* **Key Topics Covered**: Hash Functions (SHA), Symmetric/Asymmetric Encryption (AES, ElGamal), Digital Signatures, and Zero Knowledge Proof.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
