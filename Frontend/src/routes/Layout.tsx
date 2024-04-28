import React from 'react'
import Header from '../components/Header'
import { Container } from '@mui/material'
import { Outlet } from 'react-router-dom'

const Layout: React.FunctionComponent = () => {
    return (
        <>
            <Header />
            <Container maxWidth="lg" sx={{ mt: 10 }}>
                <Outlet />
            </Container>
        </>
    )
}

export default Layout