# Routes 

## Products

- **GET** - */api/products* - **WORKS** - [ ARR { JSON } ] - Auth No, Forbidden No,
- **GET** - */api/products/**{id}*** - **WORKS** - { JSON } - Auth No, Forbidden No

## Users

- *TEST USER EMAIL 1: gulordava@gmail.com/Gulordava@123 - 1.5k Salary*
- *TEST USER EMAIL 2: antique@yahoo.com/Antique@123 - 300k Salary*
- *TEST USER EMAIL 3: accountant@test.com/Accountant@123 - 10k Salary*
- **GET** - */api/users/ - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **GET** - */api/users/me - **WORKS*** - { JSON } - Auth Yes, Forbidden for others,
- **POST** - */api/users/register* - **WORKS** - { JSON } - Auth No, Forbidden No,
- **POST** - */api/users/login - **WORKS*** - { JSON } - Auth No, Forbidden No,
- **POST** - */api/users/{email}/block - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **POST** - */api/users/{email}/unblock - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **POST** - */api/users/{email}/make-accountant - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer

## Loans

- **GET** - */api/loans* - **WORKS** - [ ARR { JSON } ] - Auth YES, Forbidden Partially,
- **GET** - */api/loans/{id} - **WORKS*** - { JSON } - Auth YES, Forbidden Partially,
- **GET** - */api/loans/pending - **WORKS*** - [ ARR { JSON } ] - Auth Yes, Forbidden Partially,
- **GET** - */api/loans/declined - **WORKS*** - [ ARR { JSON } ] - Auth Yes, Forbidden Partially,
- **GET** - */api/loans/accepted - **WORKS*** - [ ARR { JSON } ] - Auth Yes, Forbidden Partially,
- **POST** - */api/loans/{id}/accept - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **POST** - */api/loans/{id}/decline - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **DELETE** - */api/loans/{id} - **WORKS*** - { JSON } - Auth Yes, Forbidden Partially

## Installment Loans

- **POST** - */api/installmentloans/new-installment - **WORKS*** - { JSON } - Auth Yes, Forbidden No,
- **PATCH** - */api/installmentloans/{id} - **WORKS*** - { JSON } - Auth Yes, Forbidden Partially

## Fast Loans

- **POST** - */api/fastloans/new-fast - **WORKS*** - { JSON } - Auth Yes, Forbidden No,
- **PATCH** - */api/fastloans/{id} - **WORKS*** - { JSON } - Auth Yes, Forbidden Partially

## Auto Loans

- **POST** - */api/autoloans/new-auto - **WORKS*** - { JSON } - Auth Yes, Forbidden No,
- **PATCH** - */api/autoloans/{id} - **WORKS*** - { JSON } - Auth Yes, Forbidden Partially

## Avaiable Products
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