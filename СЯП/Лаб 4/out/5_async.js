var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
new Promise((resolve) => {
    resolve(21);
}).then((value) => {
    console.log(value);
    return value * 2;
}).then((value) => {
    console.log(value);
});
function asyncPromise() {
    return __awaiter(this, void 0, void 0, function* () {
        let result = yield Promise.resolve(21);
        console.log(result);
        let doubleResult = result * 2;
        console.log(doubleResult);
    });
}
asyncPromise();
//# sourceMappingURL=5_async.js.map