import './App.css';
import { Route, Routes } from 'react-router-dom';
import Layout from './routes/Layout';
import Feed from './routes/pages/Feed';
import LoginPage from './routes/pages/LoginPage';
import NotFound from './routes/ErrorPages/NotFound';
import { RegisterPage } from './routes/pages/RegisterPage';

function App() {

	return (
		<Routes>
			<Route element={<Layout />}>
				<Route index element={<Feed />} />
				<Route path='/' element={<Feed />} />
				<Route path='/login' element={<LoginPage />} />
				<Route path='/register' element={<RegisterPage />} />
				<Route path='*' element={<NotFound />} />
			</Route>
		</Routes>
	);
}

export default App;
