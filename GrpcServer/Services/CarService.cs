namespace GrpcServer.Services;

using Grpc.Core;
using GrpcCarService;
using MongoDB.Bson;
using MongoDB.Driver;
using static GrpcCarService.CarService;

public class CarService : CarServiceBase
{
    private readonly IMongoCollection<Car> _cars;
    public CarService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        _cars = database.GetCollection<Car>(config["MongoDB:CollectionName"]);
    }

    public override async Task<CarResponse> CreateCar(Car request, ServerCallContext context)
    {
        request.Id = ObjectId.GenerateNewId().ToString();
        await _cars.InsertOneAsync(request);
        return await Task.FromResult(new CarResponse { Car = request });
    }

    public override Task<CarResponse> GetCarById(CarIdRequest request, ServerCallContext context)
    {
        var car = _cars.FindAsync(c => c.Id == request.Id).Result.FirstOrDefault();
        if(car == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Car not found"));

        return Task.FromResult(new CarResponse { Car = car });
    }

    public override Task<CarListResponse> GetAllCars(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new CarListResponse { Cars = { _cars.Find(_ => true).ToList() } });
    }

    public override Task<CarResponse> UpdateCar(Car request, ServerCallContext context)
    {
        var car = _cars.FindAsync(c => c.Id == request.Id).Result.FirstOrDefault();
        if(car == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Car not found"));

        _cars.ReplaceOne(c => c.Id == request.Id, request);
        return Task.FromResult(new CarResponse { Car = car });
    }

    public override Task<Empty> DeleteCar(CarIdRequest request, ServerCallContext context)
    {
        var car = _cars.FindAsync(c => c.Id == request.Id).Result.FirstOrDefault();
        if(car == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Car not found"));

        _cars.DeleteMany(c => c.Id == request.Id);
        return Task.FromResult(new Empty());
    }
}
