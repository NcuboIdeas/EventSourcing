# EventSourcing

This library allows developers to create [microservices](https://en.wikipedia.org/wiki/Microservices) with an [event sourcing](https://docs.microsoft.com/en-us/azure/architecture/patterns/event-sourcing) approach quickly and easily without the need to use any other technology or messing up the code. The developer must basically design and develop [POJO](https://en.wikipedia.org/wiki/Plain_old_Java_object)(Java) or [POCO](https://en.wikipedia.org/wiki/Plain_old_CLR_object) (CLR).

This library offers a perfect approach to create services using the [CQRS](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs) pattern and return the data as a [JSON](https://en.wikipedia.org/wiki/JSON).

## How use it.

1. Clone this library.
2. Add the dependency in your project.
3. Create a new MVC project to expose the microservice controllers.
4. If you prefer, create an additional project to develop the domain classes (POCOS).

## How to develop ypur domains clases.
Use this annotation to tell the EventSourcing this class is part of the domain and will be used to maintain the state of the system.

You must extend ```Objeto``` so that EventSourcing knows that the visible methods of this class can be used from the CQRS.
    
```
namespace GamesEngine.Business
{
    [Puppet]
    public sealed class Company : Objeto
    {
    
    ...
    
    internal Customer CustomerByName(string name)
    {
      ...
    }

    public Product GetOrCreateProductById(int id)
    {
      ...
    }
```
In the code above boths methods CustomerByPlayer and GetOrCreateProductById are visible from the EventSourcing to be used from the CQRS.

## How microservice looks like.


```
  public class CompanyController : Controller
  {
    [HttpGet("api/company/")]
    public IActionResult GetCompany(int year, string bracketId)
    {
      var result = Client.Perform($@"
        company = Company();
        print company.Name name;
        print company.CustomerByName(""Cristian"") customer;
        bed =  company.GetOrCreateProductById(1);
        bed.Name = "Bed";
        print bed.Name product;
      ");
      return result;
    }
```
There are several things to notice:
1. ```company = Company(); ``` creates a company in memory affecting the state of the server and creates a variable called ```company``` to quickly access the object. T
2. ```print``` command indicates to the EventSourcing that these values are goint to be outputs of the program. The output for this program it´s 
```
{
"customer":"Cristian",
"product":"Bed"
}
```
3. The Client.Perform is already desinged to return an IActionResult with a JSON inside.

        
