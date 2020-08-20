# FakeBank

<img src="https://github.com/Chlorine-trifluoride/FakeBank/workflows/.NET%20Core/badge.svg"/>

## Info

This is a fake banking server - client application.

The **BankAPI** uses a *Code First* style SQLite database which shares the **BankModel**-data model with the client application(s).

The communication is done over https using basic HTTP status codes and JSON.

The account creation/login uses basic client side salted SHA512 hashing to protect user passwords.
**Some** API calls are protected using the login information. Very much a work in progress.

