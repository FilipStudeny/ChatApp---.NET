import { AppBar, Button, Container, Link as MuiLink } from '@mui/material'
import React from 'react'
import { Link } from 'react-router-dom'

const Header: React.FunctionComponent = () => {
    return (
        <>
            <AppBar position="fixed" sx={{ height: 60}}>
                <Container maxWidth="lg" sx={{ display: 'flex', flexDirection: 'row' }}>

                    <MuiLink underline="none" component={Link} to={'/'} color='inherit'>
                        <Button sx={{ m: 1 }} variant='outlined' color='inherit'>
                            Home
                        </Button>
                    </MuiLink>
                    <Container sx={{ display: 'flex', justifyContent: 'flex-end' }}>

                        <MuiLink underline="none" component={Link} to={'/login'} color='inherit'>
                            <Button sx={{ m: 1 }} variant='outlined' color='inherit'>
                                Sign in
                            </Button>
                        </MuiLink>
                        <MuiLink underline="none" component={Link} to={'/register'} color='inherit'>
                            <Button sx={{ m: 1 }} variant='outlined' color='inherit'>
                                Sign up
                            </Button>
                        </MuiLink>
                    </Container>
                </Container>
            </AppBar>
        </>
    )
}

export default Header