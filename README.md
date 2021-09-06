# Project Title

Stocks

## Description

A project demonstrating various web technologies and techniques. This is a Stock application comprised of Identity Server for authentication, a React-based web client and a web-api.

## List of projects

### Stock.IdentityServer

* An ASP.Net Core server for authentication using Identity Server 4. Protects the API resources defined

### Stock.API

* An ASP.Net Core Web API project. Contains all stock data 

### Stock.API.Client

* A wrapper around the calls to the Stock.API 

### Stock.API.Tests

* Unit and Integration tests for the Stock.API.

### Stock.Models

* A project that contains all the models 

### Stock.WebClient

* A react based web client that authenticates the user using the Stock.IdentityServer and retrieves data from the Stock.API

### Stock.MarketUpdater

* A c# console application that periodically updates the Stocks data.


## How to Run

Debug the projects in the following order. 
* Stock.IdentityServer
* Stock.API
* Stock.MarketUpdater
* Stock.WebClient


