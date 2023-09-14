// See https://aka.ms/new-console-template for more information

using UserMS.Client;

IUserMsClient client = new UserMsClient();

var userById = await client.GetUserByID(1);
