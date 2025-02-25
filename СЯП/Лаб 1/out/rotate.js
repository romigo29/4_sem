function rotateArray(k, ...a) {
    const a_length = a.length;
    if (a_length === 0)
        return ' ';
    k = k % a_length;
    if (k < 0)
        k += a_length;
    const new_a = [...a.slice(-k), ...a.slice(0, -k)];
    console.log(new_a.join(", "));
}
const k = -3;
const test = [1, 2, 3, 4, 5, 6, 7];
rotateArray(k, ...test);
//# sourceMappingURL=rotate.js.map