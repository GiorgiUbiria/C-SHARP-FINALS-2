# Loan API

Welcome to the Loan API documentation. This API provides endpoints to manage loans for various products, including
installment loans, fast loans, and auto loans. Below are the available routes and their functionalities.

## Routes

### Products

- **GET** - */api/products* - Retrieves a list of available products.
- **GET** - */api/products/{id}* - Retrieves details of a specific product by its ID.

### Users

- **GET** - */api/users/* - Retrieves a list of users. Only accessible to authorized users with the appropriate
  permissions.
- **GET** - */api/users/me* - Retrieves the details of the authenticated user.
- **POST** - */api/users/register* - Registers a new user.
- **POST** - */api/users/login* - Logs in a user.
- **POST** - */api/users/{email}/block* - Blocks a user. Requires authorization and appropriate permissions.
- **POST** - */api/users/{email}/unblock* - Unblocks a user. Requires authorization and appropriate permissions.
- **POST** - */api/users/{email}/make-accountant* - Grants accountant role to a user. Requires authorization and
  appropriate permissions.

### Loans

- **GET** - */api/loans* - Retrieves a list of loans. Requires authorization.
- **GET** - */api/loans/{id}* - Retrieves details of a specific loan by its ID. Requires authorization.
- **GET** - */api/loans/pending* - Retrieves a list of pending loans. Requires authorization.
- **GET** - */api/loans/declined* - Retrieves a list of declined loans. Requires authorization.
- **GET** - */api/loans/accepted* - Retrieves a list of accepted loans. Requires authorization.
- **POST** - */api/loans/{id}/accept* - Accepts a loan. Requires authorization and appropriate permissions.
- **POST** - */api/loans/{id}/decline* - Declines a loan. Requires authorization and appropriate permissions.
- **DELETE** - */api/loans/{id}* - Deletes a loan. Requires authorization.

### Installment Loans

- **POST** - */api/installmentloans/new-installment* - Creates a new installment loan. Requires authorization.
- **PATCH** - */api/installmentloans/{id}* - Modifies an existing installment loan. Requires authorization.

### Fast Loans

- **POST** - */api/fastloans/new-fast* - Creates a new fast loan. Requires authorization.
- **PATCH** - */api/fastloans/{id}* - Modifies an existing fast loan. Requires authorization.

### Auto Loans

- **POST** - */api/autoloans/new-auto* - Creates a new auto loan. Requires authorization.
- **PATCH** - */api/autoloans/{id}* - Modifies an existing auto loan. Requires authorization.

## Available Products

```json
{
  "products": [
    {
      "id": 9,
      "title": "WD 2TB Elements Portable External Hard Drive - USB 3.0 ",
      "price": 64
    },
    {
      "id": 10,
      "title": "SanDisk SSD PLUS 1TB Internal SSD - SATA III 6 Gb/s",
      "price": 109
    },
    {
      "id": 11,
      "title": "Silicon Power 256GB SSD 3D NAND A55 SLC Cache Performance Boost SATA III 2.5",
      "price": 109
    },
    {
      "id": 12,
      "title": "WD 4TB Gaming Drive Works with Playstation 4 Portable External Hard Drive",
      "price": 114
    },
    {
      "id": 13,
      "title": "Acer SB220Q bi 21.5 inches Full HD (1920 x 1080) IPS Ultra-Thin",
      "price": 599
    },
    {
      "id": 14,
      "title": "Samsung 49-Inch CHG90 144Hz Curved Gaming Monitor (LC49HG90DMNXZA) – Super Ultrawide Screen QLED ",
      "price": 999.99
    }
  ]
}
```