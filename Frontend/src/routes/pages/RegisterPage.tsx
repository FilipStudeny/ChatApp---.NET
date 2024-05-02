import {
	SelectChangeEvent,
	Typography,
	TextField,
	FormControl,
	InputLabel,
	Select,
	MenuItem,
	Button,
	Container,
	Stack,
} from "@mui/material";
import { useState } from "react";

import type React from "react";

const RegisterPage: React.FunctionComponent = () => {
	const [gender, setGender] = useState<string>("Male");

	const handleOnGenderChange = (event: SelectChangeEvent) => {
		setGender(event.target.value as string);
	};

	return (
		<Container maxWidth='sm' sx={{ m: "auto", mt: 5 }}>
			<form>
				<Typography color='white' align='center' variant='h5' component='h5'>
					Create new account
				</Typography>
				<TextField sx={{ my: 1 }} fullWidth label='Firstname' variant='outlined' />
				<TextField sx={{ my: 1 }} fullWidth label='Lastname' variant='outlined' />
				<TextField sx={{ my: 1 }} fullWidth label='Email' variant='outlined' />
				<TextField sx={{ my: 1 }} fullWidth label='Password' variant='outlined' />
				<TextField sx={{ my: 1 }} fullWidth label='Password repeated' variant='outlined' />
				<Stack direction='row' justifyContent='space-between' sx={{ my: 1 }}>
					<FormControl>
						<InputLabel id='gender-label'>Gender</InputLabel>
						<Select
							id='gender-select'
							labelId='gender-label'
							label='Gender'
							variant='outlined'
							value={gender}
							onChange={handleOnGenderChange}
						>
							<MenuItem value='Male'>Male</MenuItem>
							<MenuItem value='Female'>Female</MenuItem>
						</Select>
					</FormControl>

					<TextField label='Age' type='number' InputLabelProps={{ shrink: true }} />
				</Stack>

				<Button sx={{ mt: 3, float: "right" }} variant='contained'>
					Create account
				</Button>
			</form>
		</Container>
	);
};

export default RegisterPage;
