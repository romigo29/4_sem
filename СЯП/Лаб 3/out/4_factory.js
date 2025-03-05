function createInstance(cls, ...args) {
    return new cls(...args);
}
class Product {
    constructor(name, price) {
        this.name = name;
        this.price = price;
    }
}
const p = createInstance(Product, "Телефон", 50000);
console.log(p);
//# sourceMappingURL=4_factory.js.map