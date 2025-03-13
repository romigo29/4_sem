new Promise((resolve) => {
    resolve(21)
}).then((value: number) => {
    console.log(value)
    return value * 2;
}).then((value: number) => {
    console.log(value);
})

async function asyncPromise() {

    let result: number = await Promise.resolve(21);
    console.log(result)

    let doubleResult: number = result * 2;
    console.log(doubleResult);
}

asyncPromise();