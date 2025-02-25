class Challenge {
    static solution(number: number) {
        if (number <= 0) {
            return 0;
        }

        let sum: number = 0;

        for (let i = 1; i < number; i++) {
            if (i % 3 == 0 || i % 5 == 0) {
                sum += i;
            }
        }

        return sum;
    }
}

let answer: number = Challenge.solution(10);
console.log(`Сумма натуральных чисел кратных 3 или 5 равна ${answer}`);