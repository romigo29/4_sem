function getMedian(a1, a2) {
    const sorted_array = a1.concat(a2).sort();
    let sorted_array_length = sorted_array.length;
    let mid = Math.floor(sorted_array_length / 2);
    return mid % 2 == 0 ? (sorted_array[mid] + sorted_array[mid - 1]) / 2 : sorted_array[mid];
}
let arr1 = [1, 3];
let arr2 = [2, 4, 6];
console.log(`Середина двух массивов: ${getMedian(arr1, arr2)}`);
//# sourceMappingURL=median.js.map