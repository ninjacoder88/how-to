// See https://aka.ms/new-console-template for more information

using HowTo.Client;

try
{
    await new Runner().GetDataAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
