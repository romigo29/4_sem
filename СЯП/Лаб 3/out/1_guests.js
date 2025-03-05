class BaseUser {
    constructor(id, name) {
        this.id = id;
        this.name = name;
    }
}
class Guest extends BaseUser {
    GetRole() {
        return "Guest";
    }
    GetPermissions() {
        return ["Просмотр контента"];
    }
}
class User extends BaseUser {
    GetRole() {
        return "User";
    }
    GetPermissions() {
        return ["Просмотр контента", "Добавление коментариев"];
    }
}
class Admin extends BaseUser {
    GetRole() {
        return "Admin";
    }
    GetPermissions() {
        return ["Просмотр контента", "Добавление коментариев",
            "Удаление комментариев", "Редактирование пользователей"
        ];
    }
}
const guest = new Guest(1, "Аноним");
console.log(guest.GetPermissions());
const admin = new Admin(2, "Мария");
console.log(admin.GetPermissions());
//# sourceMappingURL=1_guests.js.map