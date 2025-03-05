abstract class BaseUser {
    id: number;
    name: string;
    abstract GetRole(): string;
    abstract GetPermissions(): string[];

    constructor(id: number, name: string) {
        this.id = id;
        this.name = name;
    }
}

class Guest extends BaseUser {

    GetRole(): string {
        return "Guest";
    }

    GetPermissions(): string[] {
        return ["Просмотр контента"];
    }
}

class User extends BaseUser {

    GetRole(): string {
        return "User";
    }

    GetPermissions(): string[] {
        return ["Просмотр контента", "Добавление коментариев"];
    }

}

class Admin extends BaseUser {

    GetRole(): string {
        return "Admin";
    }

    GetPermissions(): string[] {
        return ["Просмотр контента", "Добавление коментариев",
            "Удаление комментариев", "Редактирование пользователей"
        ];
    }

}

const guest = new Guest(1, "Аноним");
console.log(guest.GetPermissions());

const admin = new Admin(2, "Мария");
console.log(admin.GetPermissions());