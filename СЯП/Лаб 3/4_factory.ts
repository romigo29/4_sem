function createInstance<T>(cls: new (...args: any[]) => T, ...args: any[]): T {
    return new cls(...args);
}

class Product {
    constructor(public name: string, public price: number) { }
}

const p = createInstance(Product, "Телефон", 50000);
console.log(p);