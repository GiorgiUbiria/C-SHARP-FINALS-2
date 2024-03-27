# Products
- **GET** - */api/products* - **WORKS** - [ ARR { JSON } ] - Auth No, Forbidden No,
- **GET** - */api/products/**{id}*** - **WORKS** - { JSON } - Auth No, Forbidden No

# Users 
- *TEST USER ID: 2739ca49-c5ac-46a7-86fe-0e9a2695e6b4*
- *TEST USER EMAIL: gulo@mail.ru* 
- **POST** - */api/users/register* - **WORKS** - { JSON } - Auth No, Forbidden No,
- **POST** - */api/users/login - **WORKS*** - { JSON } - Auth No, Forbidden No,
- **GET** - */api/users/me - **WORKS*** - { JSON } - Auth Yes, Forbidden for others,
- **GET** - */api/users/user - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer, 
- **GET** - */api/users/user/block - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer, 
- **GET** - */api/users/user/unblock - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer, 
- **GET** - */api/users/user/make-accountant - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer 