import React from "react"
import { useState } from 'react'
import '../css/App.css'
import { Link } from 'react-router'
import { storage } from "../localStorage/localStorage"

function Authorization() {

    const [email, setEmail] = useState('')
    const [emailInvalid, setEmailInvalid] = useState(false)
    const [emailError, setEmailError] = useState('Email не должен быть пустым')
    const [password, setPassword] = useState('')
    const [passwordInvalid, setPasswordInvalid] = useState(false)
    const [passwordError, setPasswordError] = useState('Пароль не должен быть пустым')
    const [isAuthenticated, setIsAuthenticated] = useState(false)
    const [authorizationError, setAuthorizationError] = useState('')

    const blurHandle = (e: React.ChangeEvent<HTMLInputElement>) => {
        switch (e.target.name) {

            case ('email'): {
                setEmailInvalid(true);
                break;
            }

            case ('password'): {
                setPasswordInvalid(true);
                break;
            }

        }
    }

    const handleEmail = (e: React.ChangeEvent<HTMLInputElement>) => {
        const currentEmail = e.target.value;
        setEmail(currentEmail);
        const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        if (!emailRegex.test(String(currentEmail))) {
            setEmailError("Неверно введенный email")
            return;
        }

        if (storage.emailExists(currentEmail)) {
            setEmailError("Email уже существует!");
        }

        setEmailError('')

    }

    const handlePassword = (e: React.ChangeEvent<HTMLInputElement>) => {

        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/;
        const currentPassword = e.target.value;
        setPassword(currentPassword);

        if (!passwordRegex.test(String(currentPassword))) {
            setPasswordError('Пароль должен содержать как минимум одну заглавную букву, одну строчную букву и одну цифру ')
            return;
        }
        if (currentPassword.length < 8) {
            setPasswordError('Пароль должен состоять минимум из 8 символов')
            return;
        }

        setPasswordError('');
    }



    const handleSubmit = (e: React.FormEvent<HTMLButtonElement>) => {
        e.preventDefault();

        if (!email || !password) {
            setIsAuthenticated(false);
            setAuthorizationError("Заполните все поля в форме!");
            return;
        }

        if (emailError || passwordError) {
            setIsAuthenticated(false);
            setAuthorizationError("Правильно заполните форму!");
            return;
        }

        if (!storage.emailExists(email)) {
            setIsAuthenticated(false);
            setAuthorizationError("Пользователь с таким email не существует!");
            return;
        }

        setIsAuthenticated(true);
        setAuthorizationError('');
    }


    return (
        <div className="formCard">
            <form >

                <h1>Вход</h1>
                <div className="fields">
                    {(emailInvalid && emailError) && <div className="errorMessage">{emailError}</div>}
                    <input type="email" name="email" id="" placeholder="Email" required value={email} onBlur={blurHandle} onChange={e => handleEmail(e)} />
                    {(passwordInvalid && passwordError) && <div className="errorMessage">{passwordError}</div>}
                    <input type="password" name="password" id="" placeholder="Пароль" required value={password} onBlur={blurHandle} onChange={e => handlePassword(e)} />
                    <button type="submit" onClick={e => handleSubmit(e)}>Подтвердить</button>
                </div>
                <div className="links">
                    <Link to="/sign-up">Зарегистрироваться</Link>
                    <Link to="/reset-password">Я не помню пароль</Link>
                </div>
                {(!isAuthenticated && authorizationError) && <div className="collapseMessage">{authorizationError}</div>}
                {(isAuthenticated && !authorizationError) && <div className="successMessage">Вы успешно вошли в систему! Добро пожаловать!</div>}
            </form>
        </div>
    );
}

export default Authorization
