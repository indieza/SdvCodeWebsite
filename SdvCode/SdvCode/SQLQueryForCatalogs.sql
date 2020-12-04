/*Catalog for Users*/
CREATE FULLTEXT CATALOG UsersCatalog AS DEFAULT;
CREATE FULLTEXT INDEX ON [AspNetUsers]([UserName], [FirstName], [LastName]) KEY INDEX PK_AspNetUsers;

/*Catalog for Posts*/
CREATE FULLTEXT CATALOG PostsCatalog AS DEFAULT;
CREATE FULLTEXT INDEX ON [Posts]([Title], [Content], [ShortContent]) KEY INDEX PK_Posts;

/*Catalog for Products*/
CREATE FULLTEXT CATALOG ProductsCatalog AS DEFAULT;
CREATE FULLTEXT INDEX ON [Products]([Description], [Name], [SpecificationsDescription]) KEY INDEX PK_Products;