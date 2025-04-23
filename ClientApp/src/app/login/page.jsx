'use client'
import { useState } from 'react'
import axios from 'axios'
import { useRouter } from 'next/navigation'

export default function LoginPage() {
  const router = useRouter()
  const [form, setForm] = useState({ email: '', password: '' })
  const [error, setError] = useState('')

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      const res = await axios.post('https://localhost:7001/api/auth/login', form)
      console.log('Response:', res.data);
      localStorage.setItem('token', res.data.token)
      
      const testRes = await axios.get('https://localhost:7001/api/books');
      console.log('Protected route response:', testRes.data);
      router.push('/books')
    } catch (err) {
      setError('Неверный логин или пароль')
    }
  }

  axios.interceptors.request.use(config => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });

  return (
    <div style={{ maxWidth: 400, margin: 'auto' }}>
      <h2>Вход</h2>
      <form onSubmit={handleSubmit}>
        <input name="email" placeholder="Email" onChange={handleChange} required /><br />
        <input name="password" type="password" placeholder="Пароль" onChange={handleChange} required /><br />
        {error && <p style={{ color: 'red' }}>{error}</p>}
        <button type="submit">Войти</button>
        <button onClick={() => router.push("/register")}>Регистрация</button>
      </form>
    </div>
  )
}
