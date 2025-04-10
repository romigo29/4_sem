import React from "react"
import { useState } from 'react'
import '../css/App.css'
import { Link } from 'react-router-dom'
import { storage } from "../localStorage/localStorage"

function Registration() {

  const [name, setName] = useState('')
  const [nameInvalid, setNameInvalid] = useState(false)
  const [nameError, setNameError] = useState('Имя не должно быть пустым')
  const [email, setEmail] = useState('')
  const [emailInvalid, setEmailInvalid] = useState(false)
  const [emailError, setEmailError] = useState('Email не должен быть пустым')
  const [password, setPassword] = useState('')
  const [passwordInvalid, setPasswordInvalid] = useState(false)
  const [passwordError, setPasswordError] = useState('Пароль не должен быть пустым')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [confirmPasswordInvalid, setConfirmPasswordInvalid] = useState(false)
  const [confirmPasswordError, setConfirmPasswordError] = useState('')
  const [isFormValid, setFormIsValid] = useState(false)
  const [formError, setFormError] = useState('')

  const blurHandle = (e: React.ChangeEvent<HTMLInputElement>) => {
    switch (e.target.name) {
      case ('name'): {
        setNameInvalid(true);
        break;
      }

      case ('email'): {
        setEmailInvalid(true);
        break;
      }

      case ('password'): {
        setPasswordInvalid(true);
        break;
      }

      case ('confirmPassword'): {
        setConfirmPasswordInvalid(true);
        break;
      }
    }
  }

  const handleName = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setName(value);

    const nameRegex = /^[A-Za-zА-Яа-яЁё\s]+$/;

    if (value.length > 0 && !nameRegex.test(value)) {
      setNameError("Имя должно содержать только буквы и пробелы");
      return;
    }

    if (value.length < 2 || value.length > 50) {
      setNameError("Имя должно быть от 2 до 50 символов");
      return;
    }

    setNameError('');
  };

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

  const handleConfirmPassword = (e: React.ChangeEvent<HTMLInputElement>) => {

    const currentConfirmPassword = e.target.value;
    setConfirmPassword(currentConfirmPassword);

    if (!password) {
      setConfirmPasswordError("Сначала придумайте пароль!");
      return;
    }

    if (currentConfirmPassword !== password) {
      setConfirmPasswordError("Пароли не совпадают");
      return;
    }

    setConfirmPasswordError('');
  }

  const handleSubmit = (e: React.FormEvent<HTMLButtonElement>) => {
    e.preventDefault();

    if (!name || !email || !password || !confirmPassword) {
      setFormIsValid(false);
      setFormError("Заполните все поля в форме!");
      return;
    }

    if (nameError || emailError || passwordError || confirmPasswordError) {
      setFormIsValid(false);
      setFormError("Правильно заполните форму!");
      return;
    }

    if (storage.emailExists(email)) {
      setFormIsValid(false);
      setFormError("Пользователь с таким email уже существует!");
      return;
    }

    setFormIsValid(true);
    setFormError('');
    storage.addUser({ name, email, password });
  }

  return (
    <div className="formCard">
      <form >

        <h1>Регистрация</h1>
        <div className="fields">
          {(nameInvalid && nameError) && <div className="errorMessage">{nameError}</div>}
          <input type="text" name="name" id="" placeholder="Имя" required value={name} onBlur={blurHandle} onChange={e => handleName(e)} />
          {(emailInvalid && emailError) && <div className="errorMessage">{emailError}</div>}
          <input type="email" name="email" id="" placeholder="Email" required value={email} onBlur={blurHandle} onChange={e => handleEmail(e)} />
          {(passwordInvalid && passwordError) && <div className="errorMessage">{passwordError}</div>}
          <input type="password" name="password" id="" placeholder="Пароль" required value={password} onBlur={blurHandle} onChange={e => handlePassword(e)} />
          {(confirmPasswordInvalid && confirmPasswordError) && <div className="errorMessage">{confirmPasswordError}</div>}
          <input type="password" name="confirmPassword" id="" placeholder="Подтвердите пароль" required value={confirmPassword} onBlur={blurHandle} onChange={e => handleConfirmPassword(e)} />
          <button type="submit" onClick={e => handleSubmit(e)}>Подтвердить</button>
        </div>
        <div className="links">
          <Link to="/sign-in">Войти в систему</Link>
        </div>
        {(!isFormValid && formError) && <div className="collapseMessage">{formError}</div>}
        {(isFormValid && !formError) && <div className="successMessage">Вы успешно зарегистрировались!</div>}
      </form>
    </div>
  );
}

export default Registration
