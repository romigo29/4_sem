class TCache {
    constructor() {
        this.data = new Map();
    }
    add(key, value, ttl) {
        const expireTime = Date.now() + ttl;
        this.data.set(key, { value, expireTime });
    }
    get(key) {
        const element = this.data.get(key);
        if (!element)
            return null;
        if (Date.now() > element.expireTime) {
            this.clearExpired();
            return null;
        }
        return element.value;
    }
    clearExpired() {
        const currentTime = Date.now();
        for (const [key, { expireTime }] of this.data.entries()) {
            if (currentTime > expireTime) {
                this.data.delete(key);
            }
        }
    }
}
const cache = new TCache();
cache.add("price", 100, 5000);
console.log(cache.get("price"));
setTimeout(() => console.log(cache.get("price")), 6000);
//# sourceMappingURL=3_generics.js.map