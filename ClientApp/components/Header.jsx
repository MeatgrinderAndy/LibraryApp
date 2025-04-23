'use client'
import Link from 'next/link'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { jwtDecode } from 'jwt-decode'

export default function Header() {
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const router = useRouter()
  const [isAdmin, setIsAdmin] = useState(false)

  useEffect(() => {
    const token = localStorage.getItem('token')
    setIsAuthenticated(!!token)
    if (token) {
      const decoded = jwtDecode(token)
      const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      setIsAdmin(role === 'Admin')
    }
  }, [])

  const handleLogout = () => {
    localStorage.removeItem('token')
    router.push('/login')
  }

  return (
    <header style={{ padding: 20, backgroundColor: '#f0f0f0', marginBottom: 20 }}>
      <nav style={{ display: 'flex', gap: 15 }}>
        <Link href="/books">Главная</Link>
        <Link href="/my-books">Мои книги</Link>
        {isAdmin && (
          <Link href="/create">Добавить</Link>
        )}
        {isAuthenticated ? (
          <button onClick={handleLogout} style={{ cursor: 'pointer' }}>
            Выйти
          </button>
        ) : (
          <Link href="/login">Войти</Link>
        )}
      </nav>
    </header>
  )
}
