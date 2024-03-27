# Products
- **GET** - */api/products* - **WORKS** - [ ARR { JSON } ] - Auth No, Forbidden No,
- **GET** - */api/products/**{id}*** - **WORKS** - { JSON } - Auth No, Forbidden No

# Users 
- *TEST USER EMAIL 1: gulo@mail.ru/Gulordava@123* 
- *TEST USER EMAIL 2: gulordava@gmail.com/Gulordava@123*
- **GET** - */api/users/ - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **GET** - */api/users/me - **WORKS*** - { JSON } - Auth Yes, Forbidden for others,
- **POST** - */api/users/register* - **WORKS** - { JSON } - Auth No, Forbidden No,
- **POST** - */api/users/login - **WORKS*** - { JSON } - Auth No, Forbidden No,
- **POST** - */api/users/{email}/block - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer, 
- **POST** - */api/users/{email}/unblock - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer, 
- **POST** - */api/users/{email}/make-accountant - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer
 
# Loans 
- **GET** - */api/loans* - **WORKS** - [ ARR { JSON } ] - Auth YES, Forbidden Partially,
- **GET** - */api/loans/{id} - **WORKS*** - { JSON } - Auth YES, Forbidden Partially,
- **GET** - */api/loans/pending - **WORKS*** - [ ARR { JSON } ] - Auth Yes, Forbidden Partially,
- **GET** - */api/loans/declined - **WORKS*** - [ ARR { JSON } ] - Auth Yes, Forbidden Partially,
- **GET** - */api/loans/accepted - **WORKS*** - [ ARR { JSON } ] - Auth Yes, Forbidden Partially,
- **POST** - */api/loans/{id}/accept - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **POST** - */api/loans/{id}/decline - **WORKS*** - { JSON } - Auth Yes, Forbidden for Customer,
- **DELETE** - */api/loans/{id} - **WORKS*** - { JSON } - Auth Yes, Forbidden Partially,
