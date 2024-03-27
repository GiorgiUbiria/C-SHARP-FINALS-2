# Products
- **GET** - */api/products* - **WORKS** - [ ARR { JSON } ] - Auth No, Forbidden No,
- **GET** - */api/products/**{id}*** - **WORKS** - { JSON } - Auth No, Forbidden No

# Users 
- *TEST USER EMAIL 1: gulo@mail.ru/Gulordava@123* 
- *TEST USER EMAIL 2: gulordava@gmail.com/Gulordava@123*
- **POST** - */api/users/register* - **WORKS** - { JSON } - Auth No, Forbidden No,
- **POST** - */api/users/login - **WORKS*** - { JSON } - Auth No, Forbidden No,
- **GET** - */api/users/ - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **GET** - */api/users/me - **WORKS*** - { JSON } - Auth Yes, Forbidden for others,
- **GET** - */api/users/{email}/block - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer, 
- **GET** - */api/users/{email}/unblock - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer, 
- **GET** - */api/users/{email}/make-accountant - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer
 
# Loans 
- **POST** - */api/loans* - **WORKS** - [ ARR { JSON } ] - Auth YES, Forbidden Partially,
- **POST** - */api/users/login - **WORKS*** - { JSON } - Auth No, Forbidden No,
- **GET** - */api/users/me - **WORKS*** - { JSON } - Auth Yes, Forbidden for others,
- **GET** - */api/users/ - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **GET** - */api/users/user/block - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **GET** - */api/users/user/unblock - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **GET** - */api/users/user/make-accountant - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer 
