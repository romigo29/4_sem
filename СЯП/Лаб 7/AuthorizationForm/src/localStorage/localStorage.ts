interface User {
    name: string;
    email: string;
    password: string;
}

class Storage {

    private readonly USERS_KEY = "users"

    getUsers(): User[] {
        const usersJson = localStorage.getItem(this.USERS_KEY);
        return usersJson ? JSON.parse(usersJson) : []
    }

    addUser(user: User): boolean {
        if (this.emailExists(user.email)) return false

        const users = this.getUsers();
        users.push(user);
        localStorage.setItem(this.USERS_KEY, JSON.stringify(users))
        return true;
    }

    emailExists(email: string): boolean {
        const users = this.getUsers();
        return users.some(u => u.email == email)
    }

    getPassword(email: string): string {
        const currentUser = this.getUsers().find(u => email == u.email);
        return currentUser ? currentUser.password : "пользователя не существует";
    }

}

export const storage = new Storage()