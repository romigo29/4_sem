function createPhoneNumber(p) {
    if (p.length != 10)
        return "Количество цифр в номере телефона должно быть равно 10";
    return `Набранный номер телефона: (${p.slice(0, 3).join("")}) ${p.slice(3, 6).join("")}-${p.slice(6).join("")}`;
}
let digits = [1, 2, 3, 4, 5, 6, 7, 8, 9, 4];
let result = createPhoneNumber(digits);
console.log(result);
//# sourceMappingURL=phone.js.map