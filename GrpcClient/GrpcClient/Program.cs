namespace GrpcClient;

using Grpc.Net.Client;
using GrpcCarService;
using System.Threading.Tasks;


internal class Program
{
    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5033");
        var client = new CarService.CarServiceClient(channel);

        //var newCar = new Car { Model = "Tesla", Color = "Red", Year = 2023 };
        //var createdCar = await client.CreateCarAsync(newCar);
        //Console.WriteLine($"Car created: {createdCar.Car.Model} - {createdCar.Car.Color}");

        var allCars = await client.GetAllCarsAsync(new Empty());
        Console.WriteLine("All Cars:");
        foreach(var car in allCars.Cars)
        {
            Console.WriteLine($"{car.Id}: {car.Model}, {car.Color}, {car.Year}");
        }

        //var carById = await client.GetCarByIdAsync(new CarIdRequest { Id = "67beeead8937f641706120e3" });
        //Console.WriteLine($"Car found: {carById.Car.Model}");

        //var updatedCar = await client.UpdateCarAsync(new Car { Id = "67beeead8937f641706120e3", Model = "BMW", Color = "Black", Year = 2024 });
        //Console.WriteLine($"Updated Car: {updatedCar.Car.Model}");

        //await client.DeleteCarAsync(new CarIdRequest { Id = "67beeead8937f641706120e3" });
        //Console.WriteLine("Car deleted successfully");
    }
}
