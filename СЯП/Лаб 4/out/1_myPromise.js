let myPromise = new Promise(function (resolve) {
    setTimeout(() => resolve(Math.random()), 3000);
});
myPromise.then(console.log);
function timerDelay(delay) {
    return new Promise((resolve) => {
        setTimeout(() => resolve(Math.random()), delay);
    });
}
const promises = [
    timerDelay(3000),
    timerDelay(3000),
    timerDelay(3000)
];
Promise.all(promises).then((values) => {
    console.log(values);
});
//# sourceMappingURL=1_myPromise.js.map