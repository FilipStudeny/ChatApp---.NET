import { AppBar, Button, Container } from '@mui/material'
import React from 'react'
import { Link } from 'react-router-dom'

const Header: React.FunctionComponent = () => {
    return (
        <AppBar position="fixed">
            <Container maxWidth="lg" sx={{ display: 'flex', flexDirection: 'row' }}>

                <Button sx={{ m: 1 }} variant='outlined' color='inherit' LinkComponent={Link} href='/'>Home</Button>

                <Container sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                    <Button sx={{ m: 1 }} variant='outlined' color='inherit' LinkComponent={Link} href='/login'>Sign in</Button>
                    <Button sx={{ m: 1 }} variant='outlined' color='inherit' LinkComponent={Link} href='/register'>Sign up</Button>
                </Container>

            </Container>
        </AppBar>

    )
}

export default Header