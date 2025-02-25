interface CarsType {
    manufacturer?: string,
    model?: string,
}

interface ArrayCarsType {
    cars: CarsType[]
}

const car1: CarsType = {};
car1.manufacturer = "manufacturer";
car1.model = 'model';

const car2: CarsType = {};
car2.manufacturer = "manufacturer";
car2.model = 'model';

const arrayCars: Array<ArrayCarsType> = [{
    cars: [car1, car2]
}]