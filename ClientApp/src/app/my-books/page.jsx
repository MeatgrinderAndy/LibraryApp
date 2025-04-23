'use client'
import { useEffect, useState } from 'react'
import axios from 'axios'
import {jwtDecode} from 'jwt-decode'
import Header from '../../../components/Header'
import { useRouter } from 'next/navigation'

export default function MyBooksPage() {
  const [books, setBooks] = useState([])
  const [userId, setUserId] = useState(null)

  useEffect(() => {
    const token = localStorage.getItem('token')
    if (token) {
      const decoded = jwtDecode(token)
      const nameId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
      setUserId(parseInt(nameId))
    }
  }, [])

  useEffect(() => {
    if (userId) fetchMyBooks()
  }, [userId])

  const fetchMyBooks = async () => {
    try {
      const response = await axios.get('https://localhost:7001/api/books/my', {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      })
      setBooks(response.data)
    } catch (err) {
      console.error('Ошибка при получении книг', err)
    }
  }

  const handleReturn = async (bookId) => {
    try {
      await axios.post(`https://localhost:7001/api/books/${bookId}/return`, null, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      })
      fetchMyBooks()
    } catch (err) {
      console.error('Ошибка при возврате книги', err)
    }
  }

  return (
   
    
    <div style={{ maxWidth: 600, margin: 'auto' }}>
    <Header/>
      <h2>Мои книги</h2>
      {books.length === 0 ? (
        <p>У вас нет взятых книг</p>
      ) : (
        books.map(book => (
          <div key={book.id} style={{ border: '1px solid #ccc', padding: 10, marginBottom: 10 }}>
            <p><strong>{book.title}</strong></p>
            {book.coverImage && (
            <img
            src={`data:image/jpeg;base64,${book.coverImage}`}
            alt="Обложка книги"
            style={{ width: '150px', height: 'auto', marginBottom: 10 }}
            />
           )}
            <p>ISBN: {book.isbn}</p>
            <p>Дата взятия: {book.dateWhenTaken}</p>
            <p>Вернуть до: {book.dateWhenNeedToReturn}</p>
            <button onClick={() => handleReturn(book.id)}>Вернуть книгу</button>
          </div>
        ))
      )}
      
    </div>
  )
}
