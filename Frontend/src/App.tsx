import { useState } from 'react';
import './App.css';
import reactLogo from './assets/react.svg';
import viteLogo from '/vite.svg';
import { Route, Routes } from 'react-router-dom';
import Layout from './routes/Layout';
import Feed from './routes/pages/Feed';

function App() {

    return (
        <>
		<Routes>
			<Route element={<Layout/>}>
				<Route index element={<Feed/>}/>
			</Route>
		</Routes>
         
        </>
    );
}

export default App;
