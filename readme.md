## Prerequisites

Make sure you have the following tools installed:

- [.NET SDK & Runtime](https://dotnet.microsoft.com/download) (.NET Core version 7 or higher)

## Setting up HTTPS

Before starting the development, you need to generate and trust the development certificate for HTTPS. Open a terminal or command prompt and run the following command:

```shell
dotnet dev-certs https --trust
```

## Sample Request Flow
1- Client sends an authentication request to the server with the username and password.
2- Client receives a JWT token from the server 
3- Client sends a PurchaseOrder request to the server with the JWT token from the previous step in the HTTP header.
4- Client receives a PurchaseOrder response from the server.

## Seed Data Stock Information:
-------------------------
1. Book Club Membership (SKU: MEM001)
   - Type: Subscription
   - Sale Price: $10.00

2. Video Club Membership (SKU: MEM002)
   - Type: Subscription
   - Sale Price: $10.00

3. Premium Membership (SKU: MEM003)
   - Type: Subscription
   - Sale Price: $20.00

4. Book Title (SKU: BOOK001)
   - Type: Book
   - Sale Price: $15.00

5. Video Title (SKU: VIDEO001)
   - Type: Video
   - Sale Price: $12.50


## Sample Request
```
POST https://localhost:7204/Auth/login
Content-Type: application/json

{
	"email": "user@example.com",
	"password": "password123"
}
```

```
POST https://localhost:7204/PurchaseOrder
Content-Type: application/json
{
  "products": [
    "Book Title",
    "Video Title",
    "Video Club Membership",
    "Book Club Membership",
    "Babazula Documentary"
  ]
}
```

## Sample Response
```
{
    "totalPrice": 47.50,
    "userEmail": "user@example.com",
    "shippingSlip": {
        "lines": [
            "BOOK001 - Book Title - 15.00",
            "VIDEO001 - Video Title - 12.50"
        ]
    },
    "order": {
        "id": 0,
        "cartTotal": 20.00,
        "items": [
            {
                "name": "Book Club Membership"
            },
            {
                "name": "Video Club Membership"
            }
        ],
        "customer": {
            "id": 0,
            "username": "johnDoe",
            "email": "user@example.com",
            "surname": "Doe",
            "role": "User",
            "orderHistory": [
                null
            ],
            "subscriptions": [
                "Premium Membership"
            ]
        },
        "dateTime": "2023-06-06T13:31:56.8515545+03:00"
    },
    "errors": [
        "Some of the requested products were not available in the stock and were ignored in the order"
    ]
}
```

## Sample User Accounts
```
{ 
	"email": "user@example.com",
    "password": "password123"
}
```
```
{
  "email": "janesmith@example.com",
  "password": "qwerty123"
}
```