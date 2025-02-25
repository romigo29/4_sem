function getMedian(a1: number[], a2: number[]): number {
    const sorted_array: number[] = a1.concat(a2).sort();
    let sorted_array_length = sorted_array.length;
    let mid = Math.floor(sorted_array_length / 2);

    return mid % 2 == 0 ? (sorted_array[mid] + sorted_array[mid - 1]) / 2 : sorted_array[mid];
}

let arr1: number[] = [1, 3];
let arr2: number[] = [2, 4, 6];

console.log(`Середина двух массивов: ${getMedian(arr1, arr2)}`);
