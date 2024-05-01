import { Button, Container, TextField, Typography } from "@mui/material";

const LoginPage: React.FunctionComponent = () => (
	<Container maxWidth='sm' sx={{ m: "auto", mt: 5 }}>
		<form>
			<Typography color='white' align='center' variant='h5' component='h5'>
				Sign into your account
			</Typography>
			<TextField sx={{ my: 1 }} fullWidth label='Email' variant='outlined' />
			<TextField sx={{ my: 1 }} fullWidth label='Password' variant='outlined' />
			<Button sx={{ mt: 3, float: "right" }} variant='contained' color='primary'>
				Sign in
			</Button>
		</form>
	</Container>
);

export default LoginPage;
