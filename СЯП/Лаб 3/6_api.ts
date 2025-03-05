enum HttpStatus {
    OK = 200,
    BAD_REQUEST = 400,
    NO_ACCESS = 401,
    NOT_FOUND = 404,
    INTERNAL_SERVER_ERROR = 500,
}

type ApiResponse<T> = [
    status: HttpStatus,
    data: T | null,
    error?: string,
]

function success<T>(data: T): ApiResponse<T> {
    return [HttpStatus.OK, data];
}

function error(message: string, status: HttpStatus): ApiResponse<null> {
    return [status, null, message];

}

const res1 = success({ user: "Андрей" });
console.log(res1);

const res2 = error("Не найдено", HttpStatus.NOT_FOUND);
console.log(res2);