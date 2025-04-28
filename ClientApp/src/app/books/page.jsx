'use client'

import { jwtDecode } from 'jwt-decode';
import { useEffect, useState } from 'react'
import axios from 'axios'
import Header from '../../../components/Header';
import { useRouter } from 'next/navigation';

const pageSize = 5

export default function BooksPage() {
  const [books, setBooks] = useState([])
  const [allBooks, setAllBooks] = useState([]) 
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [userId, setUserId] = useState(null)
  const [searchTerm, setSearchTerm] = useState('')
  const [genreFilter, setGenreFilter] = useState('')
  const [authorFilter, setAuthorFilter] = useState('')
  const [genres, setGenres] = useState([])
  const [authors, setAuthors] = useState([])

  const router = useRouter()

  const fetchBooks = async () => {
    try {
      const res = await axios.get('http://localhost:7001/api/books')
      setAllBooks(res.data)
      applyFilters(res.data)
      
      const uniqueGenres = [...new Set(res.data.map(book => book.genre))]
      setGenres(uniqueGenres)
      
      const uniqueAuthors = [...new Set(res.data.map(book => book.authorName))]
      setAuthors(uniqueAuthors)
    } catch (err) {
      console.error('Ошибка при загрузке книг', err)
    }
  }

  const applyFilters = (booksToFilter) => {
    let filtered = booksToFilter
    
    if (searchTerm) {
      filtered = filtered.filter(book => 
        book.title.toLowerCase().includes(searchTerm.toLowerCase())
      )
    }
    
    if (genreFilter) {
      filtered = filtered.filter(book => book.genre === genreFilter)
    }
    
    if (authorFilter) {
      filtered = filtered.filter(book => book.authorName === authorFilter)
    }
    
    setTotalPages(Math.ceil(filtered.length / pageSize))
    const start = (currentPage - 1) * pageSize
    setBooks(filtered.slice(start, start + pageSize))
  }

  useEffect(() => {
    const token = localStorage.getItem('token')
    if (token) {
      const decoded = jwtDecode(token)
      let id = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
      setUserId(id)
    }
  }, [])

  useEffect(() => {
    fetchBooks()
  }, [currentPage])

  useEffect(() => {
    if (allBooks.length > 0) {
      setCurrentPage(1) 
      applyFilters(allBooks)
    }
  }, [searchTerm, genreFilter, authorFilter])

  const handleBorrow = async (bookId) => {
    const token = localStorage.getItem('token')
    if (!token) {
      router.push('/login')
      return
    }

    try {
      await axios.post(
        `http://localhost:7001/api/books/${bookId}/borrow`,
        null,
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      )
      fetchBooks()
    } catch (err) {
      console.error('Ошибка при одалживании книги', err)
    }
  }

  const goToDetails = (bookId) => {
    router.push(`/books/${bookId}`)
  }

  return (
    <div style={{ maxWidth: 800, margin: 'auto' }}>
      <Header />
      <h2>Книги</h2>
      
      <div style={{ marginBottom: 20 }}>
        <input
          type="text"
          placeholder="Поиск по названию"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          style={{ marginRight: 10, padding: 5 }}
        />
        
        <select 
          value={genreFilter}
          onChange={(e) => setGenreFilter(e.target.value)}
          style={{ marginRight: 10, padding: 5 }}
        >
          <option value="">Все жанры</option>
          {genres.map(genre => (
            <option key={genre} value={genre}>{genre}</option>
          ))}
        </select>
        
        <select 
          value={authorFilter}
          onChange={(e) => setAuthorFilter(e.target.value)}
          style={{ marginRight: 10, padding: 5 }}
        >
          <option value="">Все авторы</option>
          {authors.map(author => (
            <option key={author} value={author}>{author}</option>
          ))}
        </select>
      </div>

      {books.map((book) => (
        <div key={book.id} style={{ border: '1px solid #ccc', padding: 10, marginBottom: 10 }}>
          <h3>{book.title}</h3>
          {book.coverImage && (
            <img
              src={`data:image/jpeg;base64,${book.coverImage}`}
              alt="Обложка книги"
              style={{ width: '150px', height: 'auto', marginBottom: 10 }}
            />
          )}

          <p><strong>ISBN:</strong> {book.isbn}</p>
          <p><strong>Автор:</strong> {book.authorName}</p>
          <p><strong>Жанр:</strong> {book.genre}</p>
          <p><strong>Статус:</strong> {book.dateWhenTaken ? 'Уже взята' : 'Доступна'}</p>

          <div style={{ display: 'flex', gap: 10 }}>
            {!book.dateWhenTaken && (
              <button onClick={() => handleBorrow(book.id)}>Взять книгу</button>
            )}
            <button onClick={() => goToDetails(book.id)}>Подробнее</button>
          </div>
        </div>
      ))}

      <div style={{ marginTop: 20 }}>
        <button onClick={() => setCurrentPage((p) => Math.max(p - 1, 1))} disabled={currentPage === 1}>
          Назад
        </button>
        <span style={{ margin: '0 10px' }}>{currentPage} / {totalPages}</span>
        <button onClick={() => setCurrentPage((p) => Math.min(p + 1, totalPages))} disabled={currentPage === totalPages}>
          Вперед
        </button>
      </div>
    </div>
  )
}