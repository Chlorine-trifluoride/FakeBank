# FakeBank <img src="https://github.com/Chlorine-trifluoride/FakeBank/raw/master/Media/icon.png" width=64/>

<img src="https://github.com/Chlorine-trifluoride/FakeBank/workflows/.NET%20Core/badge.svg"/>

## Info

This is a fake banking server - client application.

The **BankAPI** uses a *Code First* style SQLite database which shares the **BankModel**-data model with the client application(s).

The communication is done over https using basic HTTP status codes and JSON.

The account creation/login uses basic client side salted SHA512 hashing to protect user passwords.
**Some** API calls are protected using the login information. Very much a work in progress.

See the <a href="https://github.com/Chlorine-trifluoride/FakeBank/projects/1">**Projects**</a> page for more info on current progress.

## System Requirements

The API-service runs on dotnet core.

The GUI application requires Windows to run WPF.

The Web project is discontinued / not functional.

The API url:port is hard coded in **<a href=https://github.com/Chlorine-trifluoride/FakeBank/blob/master/BankClientApp/HttpMgr.cs>HttpMgr.cs</a>** and might require configuring depending on the run environment. The client also discards warnings about self signed SSL certificates.

## Account Info Page

<img src="https://github.com/Chlorine-trifluoride/FakeBank/raw/master/Media/testman_infopage.png"/>


## Transfering Funds

<img src="https://github.com/Chlorine-trifluoride/FakeBank/raw/master/Media/bill_transfer.gif"/>


## Transaction History

<img src="https://github.com/Chlorine-trifluoride/FakeBank/raw/master/Media/transaction_history.png"/>


## Basic Login / Register

<img src="https://github.com/Chlorine-trifluoride/FakeBank/raw/master/Media/login_register.png"/>
